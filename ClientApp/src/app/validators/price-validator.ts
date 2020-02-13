import { AbstractControl } from '@angular/forms';

export function priceMaxValidator(control: AbstractControl): { [key: string]: any } | null {
  return control.value <= 999 ? null : { invalidPrice: { valid: false, value: control.value } };
}
