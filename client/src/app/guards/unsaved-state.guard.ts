import { CanDeactivateFn } from '@angular/router';
import { CanComponentDeactivate } from '../utils/models/can-deactivate';

export const unsavedStateGuard: CanDeactivateFn<CanComponentDeactivate> = (component, currentRoute, currentState, nextState) => {
  console.log("Checking to deactivate");
  return component.canDeactivate ? component.canDeactivate() : true;
};
