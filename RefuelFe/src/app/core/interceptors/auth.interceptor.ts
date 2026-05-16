import { HttpErrorResponse, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { BehaviorSubject, catchError, filter, switchMap, take, throwError } from 'rxjs';

// Module-level state so a single refresh is shared across all concurrent 401s
let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

function addToken(req: HttpRequest<unknown>, token: string): HttpRequest<unknown> {
  return req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
}

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const token = authService.getToken();

  const isAuthEndpoint = req.url.includes('/login') || req.url.includes('/refresh');

  let authReq = req;
  if (token && !isAuthEndpoint) {
    authReq = addToken(req, token);
  }

  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !isAuthEndpoint) {
        // Guard against infinite loop on /refresh 401
        if (!isRefreshing) {
          isRefreshing = true;
          refreshTokenSubject.next(null);

          return authService.refreshToken().pipe(
            switchMap(res => {
              isRefreshing = false;
              refreshTokenSubject.next(res.accessToken);
              return next(addToken(req, res.accessToken));
            }),
            catchError(refreshError => {
              isRefreshing = false;
              refreshTokenSubject.next(null);
              authService.logout();
              return throwError(() => refreshError);
            })
          );
        }

        // Other requests wait for the refresh to complete, then retry with the new token
        return refreshTokenSubject.pipe(
          filter(token => token !== null),
          take(1),
          switchMap(newToken => next(addToken(req, newToken!)))
        );
      }
      return throwError(() => error);
    })
  );
};
