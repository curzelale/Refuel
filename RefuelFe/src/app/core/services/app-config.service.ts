import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap, firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AppConfigService {
  private http = inject(HttpClient);
  apiUrl = '';

  load(): Promise<void> {
    return firstValueFrom(
      this.http.get<{ apiUrl: string }>('/config.json').pipe(
        tap(config => (this.apiUrl = config.apiUrl))
      )
    ).then(() => {});
  }
}
