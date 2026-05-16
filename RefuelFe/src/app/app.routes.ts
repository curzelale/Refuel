import { Routes } from '@angular/router';
import { LayoutComponent } from './layout/layout.component';
import { LoginComponent } from './features/login/login.component';
import { AddRefuelComponent } from './features/add-refuel/add-refuel.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { VehiclesComponent } from './features/vehicles/vehicles.component';
import { GasStationsComponent } from './features/gas-stations/gas-stations.component';
import { FuelsComponent } from './features/fuels/fuels.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: '',
    component: LayoutComponent,
    canActivate: [authGuard],
    children: [
      { path: 'add-refuel', component: AddRefuelComponent },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'vehicles', component: VehiclesComponent },
      { path: 'gas-stations', component: GasStationsComponent },
      { path: 'fuels', component: FuelsComponent },
      { path: '', redirectTo: 'add-refuel', pathMatch: 'full' }
    ]
  },
  { path: '**', redirectTo: 'login' }
];
