import { HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { ToastService } from 'src/app/shared/services/toast.service';
import { Injectable } from '@angular/core';

@Injectable()
export class ErorrHandlingInterceptor implements HttpInterceptor {

  constructor(private toastService: ToastService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): any {
    return next.handle(request).pipe(
      tap(
        resp => {
          if (resp instanceof HttpResponse) {
            if (!!resp.body.errors) {
              //this.toastService.openSnackBar("Errors", resp.body.errors[0].message);
            }
          }
        },
        error => {
          if (error instanceof HttpErrorResponse) {
            //this.toasterService.error(error.message, error.name, { closeButton: true });
          }
        }
      ));
  }
}

