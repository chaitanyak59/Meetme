import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/accounts.service';
import { ToastrService } from 'ngx-toastr';

export const authGuardGuard: CanActivateFn = (route, state) => {
  var account = inject(AccountService);
  var router = inject(Router);
  var toastr = inject(ToastrService);

  if (account.isLoggedIn()?.isLoggedIn) {
    return true;
  }

  toastr.error('You must be logged in to access', 'Access Denied');
  router.navigateByUrl('/');
  return false;
};
