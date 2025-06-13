import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterOutlet } from '@angular/router'; // On importe Router
import { AuthService } from './services/auth.service'; // On importe AuthService

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  // On injecte les services dans le constructeur et on les déclare "public"
  // pour pouvoir les utiliser dans le HTML
  constructor(public authService: AuthService, public router: Router) {}

  // Notre méthode de déconnexion
  logout(): void {
    this.authService.logout(); // On appelle la méthode du service
    this.router.navigate(['/login']); // On redirige l'utilisateur vers la page de connexion
  }
}
