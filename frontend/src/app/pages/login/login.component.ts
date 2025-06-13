import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router'; // On ajoute RouterLink ici
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink], // On ajoute RouterLink ici
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  // ... le reste du code de la classe ne change pas ...
  loginData = {
    username: '',
    password: ''
  };

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(): void {
    this.authService.login(this.loginData).subscribe({
      next: () => {
        alert('Connexion réussie !');
        this.router.navigate(['/']);
      },
      error: (err) => {
        alert(`Erreur de connexion : Identifiants incorrects ou problème serveur.`);
      }
    });
  }
}
