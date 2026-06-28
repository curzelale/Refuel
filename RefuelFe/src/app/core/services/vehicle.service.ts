import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppConfigService } from './app-config.service';
import { VehicleDto, CreateVehicleRequest, RefuelDto } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class VehicleService {
  private http = inject(HttpClient);
  private config = inject(AppConfigService);
  private get apiUrl() { return `${this.config.apiUrl}/api/v1/Vehicles`; }

  getVehicles(): Observable<VehicleDto[]> {
    return this.http.get<VehicleDto[]>(this.apiUrl);
  }

  getVehicle(id: string): Observable<VehicleDto> {
    return this.http.get<VehicleDto>(`${this.apiUrl}/${id}`);
  }

  createVehicle(request: CreateVehicleRequest): Observable<VehicleDto> {
    return this.http.post<VehicleDto>(this.apiUrl, request);
  }

  updateVehicle(id: string, request: CreateVehicleRequest): Observable<VehicleDto> {
    return this.http.put<VehicleDto>(`${this.apiUrl}/${id}`, request);
  }

  deleteVehicle(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getVehicleRefuels(vehicleId: string): Observable<RefuelDto[]> {
    return this.http.get<RefuelDto[]>(`${this.apiUrl}/${vehicleId}/refuels`);
  }
}
