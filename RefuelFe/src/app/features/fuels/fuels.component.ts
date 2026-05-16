import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';

import { FuelService } from '../../core/services/fuel.service';
import { FuelDto, CreateFuelRequest } from '../../core/models/models';

@Component({
  selector: 'app-fuels',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    MatTooltipModule
  ],
  templateUrl: './fuels.component.html',
  styleUrl: './fuels.component.scss',
})
export class FuelsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private fuelService = inject(FuelService);
  private snackBar = inject(MatSnackBar);

  fuels = signal<FuelDto[]>([]);
  isSubmitting = signal(false);
  isLoading = signal(true);
  displayedColumns = ['name', 'actions'];

  fuelForm: FormGroup = this.fb.group({
    name: ['', Validators.required]
  });

  ngOnInit(): void {
    this.loadFuels();
  }

  loadFuels() {
    this.isLoading.set(true);
    this.fuelService.getFuels().subscribe({
      next: (data) => {
        this.fuels.set(data);
        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false)
    });
  }

  onSubmit() {
    if (this.fuelForm.valid) {
      this.isSubmitting.set(true);
      const request: CreateFuelRequest = this.fuelForm.value;
      this.fuelService.createFuel(request).subscribe({
        next: (created) => {
          this.isSubmitting.set(false);
          this.fuels.update(f => [...f, created]);
          this.snackBar.open('Tipo carburante aggiunto con successo!', 'Chiudi', { duration: 3000 });
          this.fuelForm.reset();
        },
        error: () => {
          this.isSubmitting.set(false);
          this.snackBar.open('Impossibile aggiungere il tipo carburante.', 'Chiudi', { duration: 3000 });
        }
      });
    }
  }

  deleteFuel(id: string) {
    this.fuelService.deleteFuel(id).subscribe({
      next: () => {
        this.fuels.update(f => f.filter(fuel => fuel.id !== id));
        this.snackBar.open('Tipo carburante eliminato.', 'Chiudi', { duration: 3000 });
      },
      error: () => this.snackBar.open('Impossibile eliminare il tipo carburante.', 'Chiudi', { duration: 3000 })
    });
  }
}
