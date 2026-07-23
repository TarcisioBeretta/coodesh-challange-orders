import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { OrderRequest } from '../models/order-request';
import { OrderResponse } from '../models/order-response';

@Injectable({
  providedIn: 'root'
})
export class OrderApi {

  private http = inject(HttpClient);

  send(order: OrderRequest): Observable<OrderResponse> {
    return this.http.post<OrderResponse>(
      'http://localhost:5000/api/orders',
      order
    );
  }
}