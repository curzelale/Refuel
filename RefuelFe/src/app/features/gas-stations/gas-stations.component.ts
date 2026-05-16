import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { forkJoin, of, switchMap } from 'rxjs';
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

import { GasStationService } from '../../core/services/gas-station.service';
import { FuelService } from '../../core/services/fuel.service';
import { GasStationDto, FuelDto, CreateGasStationRequest } from '../../core/models/models';

@Component({
  selector: 'app-gas-stations',
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
  templateUrl: './gas-stations.component.html',
  styleUrl: './gas-stations.component.scss',
})

//TODO: sistemare questo per caricare tutto in un colpo solo come per i veicoli
//TODO: Tirare su in automatico la posizione dell'utente
export class GasStationsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private gasStationService = inject(GasStationService);
  private fuelService = inject(FuelService);
  private snackBar = inject(MatSnackBar);

  stations = signal<GasStationDto[]>([]);
  fuels = signal<FuelDto[]>([]);
  isSubmitting = signal(false);
  isLoading = signal(true);
  displayedColumns = ['name', 'address', 'latitude', 'longitude', 'fuels', 'actions'];

  stationForm: FormGroup = this.fb.group({
    name: ['', Validators.required],
    address: ['', Validators.required],
    latitude: ['', [Validators.required, Validators.min(-90), Validators.max(90)]],
    longitude: ['', [Validators.required, Validators.min(-180), Validators.max(180)]],
    fuelIds: [[]]
  });

  ngOnInit(): void {
    forkJoin({
      stations: this.gasStationService.getGasStations(),
      fuels: this.fuelService.getFuels()
    }).subscribe({
      next: ({ stations, fuels }) => {
        this.stations.set(stations);
        this.fuels.set(fuels);
        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false)
    });
  }

  onSubmit() {
    if (this.stationForm.valid) {
      this.isSubmitting.set(true);
      const { fuelIds, ...formValues } = this.stationForm.value;
      const request: CreateGasStationRequest = {
        ...formValues,
        latitude: parseFloat(formValues.latitude),
        longitude: parseFloat(formValues.longitude)
      };
      const selectedFuelIds: string[] = fuelIds ?? [];

      this.gasStationService.createGasStation(request).pipe(
        switchMap(created => {
          if (selectedFuelIds.length === 0) {
            return of(created);
          }
          return forkJoin(
            selectedFuelIds.map(fuelId => this.gasStationService.addFuelToGasStation(created.id, fuelId))
          ).pipe(
            switchMap(() => this.gasStationService.getGasStation(created.id))
          );
        })
      ).subscribe({
        next: (finalStation) => {
          this.isSubmitting.set(false);
          this.stations.update(s => [...s, finalStation]);
          this.snackBar.open('Distributore aggiunto con successo!', 'Chiudi', { duration: 3000 });
          this.stationForm.reset();
        },
        error: () => {
          this.isSubmitting.set(false);
          this.snackBar.open('Impossibile aggiungere il distributore.', 'Chiudi', { duration: 3000 });
        }
      });
    }
  }

  deleteStation(id: string) {
    this.gasStationService.deleteGasStation(id).subscribe({
      next: () => {
        this.stations.update(s => s.filter(station => station.id !== id));
        this.snackBar.open('Distributore eliminato.', 'Chiudi', { duration: 3000 });
      },
      error: () => this.snackBar.open('Impossibile eliminare il distributore.', 'Chiudi', { duration: 3000 })
    });
  }
}
