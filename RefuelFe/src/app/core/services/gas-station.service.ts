import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppConfigService } from './app-config.service';
import { GasStationDto, CreateGasStationRequest, FuelDto } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class GasStationService {
  private http = inject(HttpClient);
  private config = inject(AppConfigService);
  private get apiUrl() { return `${this.config.apiUrl}/api/v1/GasStations`; }

  getGasStations(): Observable<GasStationDto[]> {
    return this.http.get<GasStationDto[]>(this.apiUrl);
  }

  getGasStation(id: string): Observable<GasStationDto> {
    return this.http.get<GasStationDto>(`${this.apiUrl}/${id}`);
  }

  createGasStation(request: CreateGasStationRequest): Observable<GasStationDto> {
    return this.http.post<GasStationDto>(this.apiUrl, request);
  }

  updateGasStation(id: string, request: CreateGasStationRequest): Observable<GasStationDto> {
    return this.http.put<GasStationDto>(`${this.apiUrl}/${id}`, request);
  }

  deleteGasStation(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getGasStationFuels(id: string): Observable<FuelDto[]> {
    return this.http.get<FuelDto[]>(`${this.apiUrl}/${id}/fuels`);
  }

  addFuelToGasStation(id: string, fuelId: string): Observable<GasStationDto> {
    return this.http.put<GasStationDto>(`${this.apiUrl}/${id}/fuels/${fuelId}`, {});
  }
}
