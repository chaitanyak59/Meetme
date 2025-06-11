import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

import { routes } from './app.routes';
import { HttpClient, provideHttpClient } from '@angular/common/http';
import { APP_CONFIG } from './utils/config';
import { environment } from '../environments/environment';
import { provideToastr } from 'ngx-toastr';

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimations(),
    provideToastr({
      timeOut: 2000,
      positionClass: 'toast-bottom-right',
      closeButton: true,
      progressBar: true
    }),
    provideHttpClient(),
    provideRouter(routes),
    {
      provide: APP_CONFIG,
      useValue: environment
    }
  ]
};
