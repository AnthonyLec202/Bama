import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

// On définit une interface pour dire à quoi ressemble un Burger côté frontend
export interface Burger {
  burgerId: number;
  name: string;
  description: string;
  price: number;
  stock: number;
  imageUrl: string;
}

@Injectable({
  providedIn: 'root'
})
export class BurgerService {
  // L'URL de notre backend que nous avons testée plus tôt
  private apiUrl = 'http://localhost:5218/api/burgers';

  constructor(private http: HttpClient) { }

  // Méthode pour récupérer tous les burgers
  getBurgers(): Observable<Burger[]> {
    return this.http.get<Burger[]>(this.apiUrl);
  }
}
