import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs/operators';

export const errorsInterceptor: HttpInterceptorFn = (req, next) => {
 
  const router = inject(Router);
  const toastr = inject(ToastrService)

  return next(req).pipe(catchError((error) => {
    if (error) {
      switch (error.status) {
        case 400:
          if (error.error.errors || error.errors) {
            var errs = error.error.errors || error.errors;
            const modelErrors = [];
            for (const key in errs) {
              if (errs[key]) {
                modelErrors.push(errs[key]);
              }
            }
            throw modelErrors.flat();
          } else {
            toastr.error(error.error, error.status);
          }
          break;
        case 401:
          toastr.error(error.error, 'Unauthorized');
          break;
        case 404:
          router.navigateByUrl('/not-found');
          break;
        case 500:
          router.navigateByUrl('/server-error', {
            state: { error: error.error }
          })
          break;
        default:
          toastr.error('Something went wrong', 'Error');
          break
      }
    }
    throw error;
  }));
};
