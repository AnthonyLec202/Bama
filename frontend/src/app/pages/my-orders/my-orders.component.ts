import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderService, CustomerOrder } from '../../services/order.service';

@Component({
  selector: 'app-my-orders',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my-orders.component.html',
  styleUrls: ['./my-orders.component.css']
})
export class MyOrdersComponent implements OnInit {
  orders: CustomerOrder[] = [];
  isLoading = true;

  constructor(private orderService: OrderService) { }

  ngOnInit(): void {
    this.orderService.getAllOrders().subscribe({
      next: (data) => {
        this.orders = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error("Erreur lors de la récupération de l'historique", err);
        this.isLoading = false;
      }
    });
  }
}
