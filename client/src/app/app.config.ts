import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

import { routes } from './app.routes';
import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import { APP_CONFIG } from './utils/config';
import { environment } from '../environments/environment';
import { provideToastr } from 'ngx-toastr';
import { errorsInterceptor } from './errors.interceptor';
import { GALLERY_CONFIG, GalleryConfig } from 'ng-gallery';

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimations(),
    provideToastr({
      timeOut: 2000,
      positionClass: 'toast-bottom-right',
      progressBar: true
    }),
    provideHttpClient(withInterceptors([errorsInterceptor])),
    provideRouter(routes),
    {
      provide: APP_CONFIG,
      useValue: environment
    },
    {
      provide: GALLERY_CONFIG,
      useValue: {
        autoHeight: true,
      } as GalleryConfig
    }
  ]
};
