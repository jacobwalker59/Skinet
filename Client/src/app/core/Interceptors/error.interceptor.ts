import { Injectable } from '@angular/core';
import {HttpInterceptor, HttpRequest, HttpHandler, HttpEvent} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Router, NavigationExtras } from '@angular/router';
import {catchError, delay} from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private router: Router, private toastr: ToastrService) {}

  intercept( req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
       
        catchError(error => {
            if (error) {
                if(error.status === 400){
                    if (error.error.errors)
                    {
                        throw error.error;
                    } else {
                    this.toastr.error(error.error.message, error.error.statusCode);
                    }
                }
                if(error.status === 401){
                    this.toastr.error(error.error.message, error.error.statusCode);
                }
                if (error.status === 404) {
                    this.router.navigateByUrl('/not-found');
                    // redirects using our router in the not found component
                }
                if (error.status === 500) {
                    const navigationExtras: NavigationExtras = {state: {error: error.error} };
                    this.router.navigateByUrl('/server-error', navigationExtras);
                    
                    // redirects using our router in the not found component
                }
            }
            return throwError(error);
        })
    );
    // allows us to intercept the http outgoing request
  }
}
