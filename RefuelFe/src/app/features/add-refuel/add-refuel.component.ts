import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { forkJoin } from 'rxjs';

import { VehicleService } from '../../core/services/vehicle.service';
import { GasStationService } from '../../core/services/gas-station.service';
import { FuelService } from '../../core/services/fuel.service';
import { RefuelService } from '../../core/services/refuel.service';
import { VehicleDto, GasStationDto, FuelDto } from '../../core/models/models';

@Component({
  selector: 'app-add-refuel',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSnackBarModule
  ],
  templateUrl: './add-refuel.component.html',
  styleUrl: './add-refuel.component.scss',
})


//TODO: sistemare i valori nel pannello per mostrare solo le voci compatibili tra loro
//TODO: Mostrare i distributori in ordine di vicinanza con la distanza in metri
export class AddRefuelComponent implements OnInit {
  private fb = inject(FormBuilder);
  private vehicleService = inject(VehicleService);
  private gasStationService = inject(GasStationService);
  private fuelService = inject(FuelService);
  private refuelService = inject(RefuelService);
  private snackBar = inject(MatSnackBar);

  // Fix #4: use signals so Zoneless rendering updates the select options correctly
  vehicles = signal<VehicleDto[]>([]);
  gasStations = signal<GasStationDto[]>([]);
  fuels = signal<FuelDto[]>([]);

  refuelForm: FormGroup = this.fb.group({
    vehicleId: ['', Validators.required],
    gasStationId: ['', Validators.required],
    fuelId: ['', Validators.required],
    quantity: ['', [Validators.required, Validators.min(0.1)]],
    totalPrice: ['', [Validators.required, Validators.min(0.1)]],
    date: [new Date(), Validators.required],
    odometerKm: ['', [Validators.required, Validators.min(0)]],
    note: ['']
  });

  isLoading = signal(false);

  ngOnInit(): void {
    this.loadData();
  }

  // Fix #5: use forkJoin to load all data in parallel with a single subscription
  loadData() {
    forkJoin({
      vehicles: this.vehicleService.getVehicles(),
      stations: this.gasStationService.getGasStations(),
      fuels: this.fuelService.getFuels()
    }).subscribe(({ vehicles, stations, fuels }) => {
      this.vehicles.set(vehicles);
      this.gasStations.set(stations);
      this.fuels.set(fuels);
    });
  }

  onSubmit() {
    if (this.refuelForm.valid) {
      this.isLoading.set(true);
      const formValue = this.refuelForm.value;
      const request = {
        ...formValue,
        date: new Date(formValue.date).toISOString()
      };

      this.refuelService.createRefuel(request).subscribe({
        next: () => {
          this.isLoading.set(false);
          this.snackBar.open('Rifornimento salvato con successo!', 'Chiudi', { duration: 3000 });
          // Fix #6: reset() already marks controls as pristine/untouched — no need for setErrors(null)
          this.refuelForm.reset({ date: new Date() });
        },
        error: (err) => {
          this.isLoading.set(false);
          this.snackBar.open('Impossibile salvare il rifornimento.', 'Chiudi', { duration: 3000 });
          console.error(err);
        }
      });
    }
  }
}
