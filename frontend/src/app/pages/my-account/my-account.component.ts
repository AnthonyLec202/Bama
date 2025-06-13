import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-my-account',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './my-account.component.html',
  styleUrls: ['./my-account.component.css']
})
export class MyAccountComponent implements OnInit {
  // On prépare un objet pour stocker les données du formulaire
  userProfile = {
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: ''
  };

  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    // Au chargement de la page, on récupère les infos de l'utilisateur
    this.authService.getUserProfile().subscribe({
      next: (data) => {
        // On remplit le formulaire avec les données reçues
        this.userProfile = data;
      },
      error: (err) => console.error(err)
    });
  }

  // Méthode appelée quand on soumet le formulaire
  onSubmit(): void {
    this.authService.updateUserProfile(this.userProfile).subscribe({
      next: () => {
        alert('Vos informations ont été mises à jour avec succès !');
      },
      error: (err) => {
        alert('Une erreur est survenue lors de la mise à jour.');
        console.error(err);
      }
    });
  }
}
