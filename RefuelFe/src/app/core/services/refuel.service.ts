import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { RefuelDto, CreateRefuelRequest } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class RefuelService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/v1/Refuels`;

  getRefuels(): Observable<RefuelDto[]> {
    return this.http.get<RefuelDto[]>(this.apiUrl);
  }

  getRefuel(id: string): Observable<RefuelDto> {
    return this.http.get<RefuelDto>(`${this.apiUrl}/${id}`);
  }

  createRefuel(request: CreateRefuelRequest): Observable<RefuelDto> {
    return this.http.post<RefuelDto>(this.apiUrl, request);
  }

  updateRefuel(id: string, request: CreateRefuelRequest): Observable<RefuelDto> {
    return this.http.put<RefuelDto>(`${this.apiUrl}/${id}`, request);
  }

  deleteRefuel(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
