import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';

import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';

import { OrderApi } from '../../services/order-api';
import { OrderRequest } from '../../models/order-request';
import { OrderResponse } from '../../models/order-response';

@Component({
  selector: 'app-order-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule
  ],
  templateUrl: './order-form.html',
  styleUrl: './order-form.scss'
})
export class OrderForm {
  private readonly formBuilder = inject(FormBuilder);
  private readonly orderApi = inject(OrderApi);

  response?: OrderResponse;

  readonly form = this.formBuilder.group({
    symbol: [null as string | null, Validators.required],
    side: [null as number | null, Validators.required],
    quantity: [
      null as number | null,
      [
        Validators.required,
        Validators.min(1),
        Validators.max(99999)
      ]
    ],
    price: [
      null as number | null,
      [
        Validators.required,
        Validators.min(0.01),
        Validators.max(999.99)
      ]
    ]
  });

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const order: OrderRequest = {
      symbol: this.form.value.symbol!,
      side: this.form.value.side!,
      quantity: this.form.value.quantity!,
      price: this.form.value.price!
    };

    this.orderApi.send(order).subscribe(response => {
      this.response = response;
    });
  }
}