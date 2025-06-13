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
    this.loadBurgers();
    if (this.authService.isLoggedIn()) {
      this.loadOrder();
    }
  }

  // Méthode pour charger les burgers (que nous avions oubliée)
  loadBurgers(): void {
    this.isLoading = true;
    this.burgerService.getBurgers().subscribe(data => {
      this.burgers = data;
      this.isLoading = false;
    });
  }

  // Méthode pour charger le panier
  loadOrder(): void {
    this.orderService.getOrder().subscribe(data => {
      this.order = data;
    });
  }

  // Méthode pour ajouter au panier
  addToCart(burger: Burger): void {
    if (!this.authService.isLoggedIn()) {
      alert('Vous devez être connecté pour ajouter un article au panier.');
      return;
    }
    const item: OrderItemPayload = { burgerId: burger.burgerId, quantity: 1, price: burger.price };
    this.orderService.addItemToOrder(item).subscribe({
      next: () => {
        alert(`'${burger.name}' a été ajouté à votre panier !`);
        this.loadOrder();
        this.loadBurgers();
      },
      error: (err: any) => {
        console.error(err);
        alert(`Une erreur est survenue lors de l'ajout au panier.`);
      }
    });
  }

  // Méthode pour finaliser la commande
  finalizeOrder(): void {
    if (!this.order || this.order.orderItems.length === 0) {
      alert("Votre panier est vide.");
      return;
    }
    if (confirm("Êtes-vous sûr de vouloir valider cette commande ?")) {
      this.orderService.finalizeOrder().subscribe({
        next: () => {
          alert("Commande validée avec succès !");
          this.loadOrder();
          this.loadBurgers();
        },
        error: (err: any) => {
          console.error(err);
          alert("Une erreur est survenue lors de la validation de la commande.");
        }
      });
    }
  }

  // Méthode pour vider le panier (correctement placée)
  clearCart(): void {
    if (!this.order || this.order.orderItems.length === 0) {
      return;
    }
    if (confirm("Êtes-vous sûr de vouloir vider votre panier ?")) {
      this.orderService.clearOrder().subscribe({
        next: () => {
          alert("Votre panier a été vidé.");
          this.loadOrder();
          this.loadBurgers();
        },
        error: (err: any) => {
          console.error(err);
          alert("Une erreur est survenue.");
        }
      });
    }
  }
}
