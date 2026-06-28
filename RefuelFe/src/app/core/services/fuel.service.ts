import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppConfigService } from './app-config.service';
import { FuelDto, CreateFuelRequest } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class FuelService {
  private http = inject(HttpClient);
  private config = inject(AppConfigService);
  private get apiUrl() { return `${this.config.apiUrl}/api/v1/Fuels`; }

  getFuels(): Observable<FuelDto[]> {
    return this.http.get<FuelDto[]>(this.apiUrl);
  }

  getFuel(id: string): Observable<FuelDto> {
    return this.http.get<FuelDto>(`${this.apiUrl}/${id}`);
  }

  createFuel(request: CreateFuelRequest): Observable<FuelDto> {
    return this.http.post<FuelDto>(this.apiUrl, request);
  }

  updateFuel(id: string, request: CreateFuelRequest): Observable<FuelDto> {
    return this.http.put<FuelDto>(`${this.apiUrl}/${id}`, request);
  }

  deleteFuel(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
