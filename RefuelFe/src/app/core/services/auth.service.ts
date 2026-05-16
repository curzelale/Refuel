import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginRequest, AccessTokenResponse, RefreshRequest } from '../models/models';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);

  private readonly TOKEN_KEY = 'access_token';
  private readonly REFRESH_TOKEN_KEY = 'refresh_token';
  private readonly TOKEN_EXPIRY_KEY = 'token_expiry';

  login(request: LoginRequest): Observable<AccessTokenResponse> {
    return this.http.post<AccessTokenResponse>(`${environment.apiUrl}/login`, request).pipe(
      tap(response => this.setTokens(response))
    );
  }

  refreshToken(): Observable<AccessTokenResponse> {
    const request: RefreshRequest = { refreshToken: this.getRefreshToken() || '' };
    return this.http.post<AccessTokenResponse>(`${environment.apiUrl}/refresh`, request).pipe(
      tap(response => this.setTokens(response))
    );
  }

  logout() {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
    localStorage.removeItem(this.TOKEN_EXPIRY_KEY);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_TOKEN_KEY);
  }

  /** Returns true if a non-expired token exists. */
  hasValidToken(): boolean {
    const token = this.getToken();
    if (!token) return false;

    const expiry = localStorage.getItem(this.TOKEN_EXPIRY_KEY);
    if (!expiry) return true; // No expiry stored — assume valid for backwards compatibility

    return Date.now() < parseInt(expiry, 10);
  }

  private setTokens(response: AccessTokenResponse) {
    if (response.accessToken) {
      localStorage.setItem(this.TOKEN_KEY, response.accessToken);
    }
    if (response.refreshToken) {
      localStorage.setItem(this.REFRESH_TOKEN_KEY, response.refreshToken);
    }
    if (response.expiresIn) {
      // Store absolute expiry timestamp in ms (subtract 30s as safety buffer)
      const expiryMs = Date.now() + (response.expiresIn - 30) * 1000;
      localStorage.setItem(this.TOKEN_EXPIRY_KEY, expiryMs.toString());
    }
  }
}
