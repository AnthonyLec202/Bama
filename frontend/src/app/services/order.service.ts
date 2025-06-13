import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Burger } from './burger.service';

export interface OrderItem {
  burger: Burger;
  quantity: number;
  price: number;
}

// L'interface pour la commande complète
export interface CustomerOrder {
  customerOrderId: number;
  totalPrice: number;
  orderItems: OrderItem[];
  status: string; // <-- LA CORRECTION EST ICI
}

export interface OrderItemPayload {
  burgerId: number;
  quantity: number;
  price: number;
}

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private orderApiUrl = 'http://localhost:5218/api/customers/me/order';

  constructor(private http: HttpClient) { }

  getOrder(): Observable<CustomerOrder> {
    return this.http.get<CustomerOrder>(this.orderApiUrl);
  }

  addItemToOrder(item: OrderItemPayload): Observable<any> {
    return this.http.post(`${this.orderApiUrl}/items`, [item]);
  }

  finalizeOrder(): Observable<any> {
    return this.http.post(`${this.orderApiUrl}/finalize`, {});
  }

  clearOrder(): Observable<any> {
    return this.http.delete(`${this.orderApiUrl}/items`);
  }

  // Méthode pour récupérer TOUTES les commandes
  getAllOrders(): Observable<CustomerOrder[]> {
    return this.http.get<CustomerOrder[]>(`${this.orderApiUrl}s`); // Note le 's' pour appeler /orders
  }
}
