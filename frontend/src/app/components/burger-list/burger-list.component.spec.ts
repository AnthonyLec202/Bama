import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Burger, BurgerService } from '../../services/burger.service';
import { OrderService, CustomerOrder, OrderItemPayload } from '../../services/order.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-burger-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './burger-list.component.html',
  styleUrls: ['./burger-list.component.css']
})
export class BurgerListComponent implements OnInit {
  burgers: Burger[] = [];
  order: CustomerOrder | null = null;
  isLoading = true;

  constructor(
    private burgerService: BurgerService,
    private orderService: OrderService,
    public authService: AuthService
  ) { }

  ngOnInit(): void {
    this.loadBurgers(); // On charge les burgers au démarrage
    if (this.authService.isLoggedIn()) {
      this.loadOrder(); // Et le panier si on est connecté
    }
  }

  loadBurgers(): void {
    this.isLoading = true;
    this.burgerService.getBurgers().subscribe(data => {
      this.burgers = data;
      this.isLoading = false;
    });
  }

  loadOrder(): void {
    this.orderService.getOrder().subscribe(data => {
      this.order = data;
    });
  }

  addToCart(burger: Burger): void {
    if (!this.authService.isLoggedIn()) {
      alert('Vous devez être connecté pour ajouter un article au panier.');
      return;
    }

    const item: OrderItemPayload = {
      burgerId: burger.burgerId,
      quantity: 1,
      price: burger.price
    };

    this.orderService.addItemToOrder(item).subscribe({
      next: () => {
        alert(`'${burger.name}' a été ajouté à votre panier !`);
        // ---- LA CORRECTION EST ICI ----
        // Après le succès, on recharge le panier ET la liste des burgers
        this.loadOrder();
        this.loadBurgers();
        // -----------------------------
      },
      error: (err: any) => {
        console.error(err);
        alert(`Une erreur est survenue lors de l'ajout au panier.`);
      }
    });
  }
}
