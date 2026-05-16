import { Component, inject, OnInit, signal, computed, DestroyRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTableModule } from '@angular/material/table';
import { ReactiveFormsModule, FormControl } from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

import { VehicleService } from '../../core/services/vehicle.service';
import { VehicleDto, RefuelDto } from '../../core/models/models';

@Component({
  selector: 'app-dashboard',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatSelectModule,
    MatFormFieldModule,
    MatTableModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
})
export class DashboardComponent implements OnInit {
  private vehicleService = inject(VehicleService);
  // Fix #1: inject DestroyRef to scope the subscription lifetime
  private destroyRef = inject(DestroyRef);

  // Fix #4: all state as Signals for correct Zoneless rendering
  vehicles = signal<VehicleDto[]>([]);
  refuels = signal<RefuelDto[]>([]);
  vehicleControl = new FormControl<string>('');

  displayedColumns: string[] = ['date', 'station', 'fuel', 'quantity', 'price', 'odometer'];

  // Fix #4: computed stats derived directly from the refuels signal
  totalSpent = computed(() => this.refuels().reduce((acc, r) => acc + r.totalPrice, 0));
  totalLiters = computed(() => this.refuels().reduce((acc, r) => acc + r.quantity, 0));

  ngOnInit(): void {
    this.vehicleService.getVehicles().subscribe(res => {
      this.vehicles.set(res);
      if (res.length > 0) {
        this.vehicleControl.setValue(res[0].id);
        this.loadRefuels(res[0].id);
      }
    });

    // Fix #1: use takeUntilDestroyed to prevent subscription leak
    this.vehicleControl.valueChanges.pipe(
      takeUntilDestroyed(this.destroyRef)
    ).subscribe(vehicleId => {
      if (vehicleId) {
        this.loadRefuels(vehicleId);
      }
    });
  }

  loadRefuels(vehicleId: string) {
    this.vehicleService.getVehicleRefuels(vehicleId).subscribe(res => {
      this.refuels.set(
        [...res].sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime())
      );
    });
  }
}
