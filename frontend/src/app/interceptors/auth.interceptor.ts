import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // On injecte notre service d'authentification pour accéder au token
  const authService = inject(AuthService);
  const authToken = authService.getToken();

  // Si un token existe, on clone la requête sortante et on y ajoute l'en-tête d'autorisation
  if (authToken) {
    const authReq = req.clone({
      setHeaders: {
        Authorization: `Bearer ${authToken}`
      }
    });
    // On laisse la requête modifiée continuer son chemin
    return next(authReq);
  }

  // Si pas de token, on laisse la requête originale passer sans la modifier
  return next(req);
};
