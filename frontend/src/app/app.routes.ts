import { Routes } from '@angular/router';
import { BurgerListComponent } from './components/burger-list/burger-list.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { MyAccountComponent } from './pages/my-account/my-account.component';
import { MyOrdersComponent } from './pages/my-orders/my-orders.component'; // Assure-toi que l'import est là
import { authGuard } from './guards/auth.guard'; // Assure-toi que l'import est là

export const routes: Routes = [
  { path: '', component: BurgerListComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  // On protège les routes qui nécessitent d'être connecté
  { path: 'mon-compte', component: MyAccountComponent, canActivate: [authGuard] },
  { path: 'mes-commandes', component: MyOrdersComponent, canActivate: [authGuard] } // <-- LA CORRECTION EST ICI
];
