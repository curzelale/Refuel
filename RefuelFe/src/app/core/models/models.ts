export interface FuelDto {
  id: string;
  name?: string;
}

export interface GasStationDto {
  id: string;
  name?: string;
  address?: string;
  latitude: number;
  longitude: number;
  fuels?: FuelDto[];
}

export interface LoginRequest {
  email?: string;
  password?: string;
  twoFactorCode?: string;
  twoFactorRecoveryCode?: string;
}

export interface RefreshRequest {
  refreshToken?: string;
}

export interface VehicleDto {
  id: string;
  brand?: string;
  model?: string;
  owner?: string;
  fuels?: FuelDto[];
  nickname?: string;
  licencesPlate?: string;
}

export interface RefuelDto {
  id: string;
  vehicleId: string;
  gasStationId: string;
  fuelId: string;
  quantity: number;
  totalPrice: number;
  date: string;
  odometerKm: number;
  note?: string;
  vehicle?: VehicleDto;
  gasStation?: GasStationDto;
  fuel?: FuelDto;
}

export interface CreateRefuelRequest {
  vehicleId: string;
  gasStationId: string;
  fuelId: string;
  quantity: number;
  totalPrice: number;
  date: string;
  odometerKm: number;
  note?: string;
}

export interface CreateVehicleRequest {
  brand: string;
  model: string;
  licencesPlate?: string;
  nickname?: string;
  owner?: string;
  fuelIds?: string[];
}

export interface CreateGasStationRequest {
  name: string;
  address: string;
  latitude: number;
  longitude: number;
}

export interface CreateFuelRequest {
  name: string;
}

export interface AccessTokenResponse {
  tokenType: string;
  accessToken: string;
  expiresIn: number;
  refreshToken: string;
}
