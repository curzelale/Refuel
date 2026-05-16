import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDividerModule } from '@angular/material/divider';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';

import { VehicleService } from '../../core/services/vehicle.service';
import { FuelService } from '../../core/services/fuel.service';
import { VehicleDto, FuelDto, CreateVehicleRequest } from '../../core/models/models';

@Component({
  selector: 'app-vehicles',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatSnackBarModule,
    MatDividerModule,
    MatProgressSpinnerModule,
    MatTooltipModule
  ],
  templateUrl: './vehicles.component.html',
  styleUrl: './vehicles.component.scss',
})
export class VehiclesComponent implements OnInit {
  private fb = inject(FormBuilder);
  private vehicleService = inject(VehicleService);
  private fuelService = inject(FuelService);
  private snackBar = inject(MatSnackBar);

  vehicles = signal<VehicleDto[]>([]);
  fuels = signal<FuelDto[]>([]);
  isSubmitting = signal(false);
  isLoading = signal(true);
  displayedColumns = ['brand', 'model', 'nickname', 'licencesPlate', 'fuels', 'actions'];

  vehicleForm: FormGroup = this.fb.group({
    brand: ['', Validators.required],
    model: ['', Validators.required],
    nickname: [''],
    licencesPlate: [''],
    owner: [''],
    fuelIds: [[]]
  });

  ngOnInit(): void {
    forkJoin({
      vehicles: this.vehicleService.getVehicles(),
      fuels: this.fuelService.getFuels()
    }).subscribe({
      next: ({ vehicles, fuels }) => {
        this.vehicles.set(vehicles);
        this.fuels.set(fuels);
        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false)
    });
  }

  onSubmit() {
    if (this.vehicleForm.valid) {
      this.isSubmitting.set(true);
      const { fuelIds, ...rest } = this.vehicleForm.value;
      const request: CreateVehicleRequest = {
        ...rest,
        fuelIds: fuelIds ?? []
      };
      this.vehicleService.createVehicle(request).subscribe({
        next: (created) => {
          this.isSubmitting.set(false);
          this.vehicles.update(v => [...v, created]);
          this.snackBar.open('Veicolo aggiunto con successo!', 'Chiudi', { duration: 3000 });
          this.vehicleForm.reset();
        },
        error: () => {
          this.isSubmitting.set(false);
          this.snackBar.open('Impossibile aggiungere il veicolo.', 'Chiudi', { duration: 3000 });
        }
      });
    }
  }

  deleteVehicle(id: string) {
    this.vehicleService.deleteVehicle(id).subscribe({
      next: () => {
        this.vehicles.update(v => v.filter(vehicle => vehicle.id !== id));
        this.snackBar.open('Veicolo eliminato.', 'Chiudi', { duration: 3000 });
      },
      error: () => this.snackBar.open('Impossibile eliminare il veicolo.', 'Chiudi', { duration: 3000 })
    });
  }
}
