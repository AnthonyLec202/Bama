import { ApplicationConfig } from '@angular/core';
import { provideRouter, withInMemoryScrolling } from '@angular/router';
// On importe 'withInterceptors'
import { provideHttpClient, withFetch, withInterceptors } from '@angular/common/http';
import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';

// On importe notre nouvel intercepteur
import { authInterceptor } from './interceptors/auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes, withInMemoryScrolling({
       anchorScrolling: 'enabled'
    })),
    provideClientHydration(),
    // On dit à HttpClient d'utiliser le fetch ET nos intercepteurs
    provideHttpClient(withFetch(), withInterceptors([authInterceptor]))
  ]
};
