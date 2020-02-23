import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpErrorResponse } from '@angular/common/http';
import { catchError, tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Product } from '../interfaces/product';
import { ToastService } from './toast.service';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private productApi = `${environment.api}`;
  constructor(private httpClient: HttpClient, private toastService: ToastService) { }

  /**
  * GET: get all products from the database
  */
  getAllProducts(): Observable<Product[]> {
    return this.httpClient
      .get<Product[]>(`${this.productApi}getAllProducts`)
      .pipe(tap(_ => this.notify('getAllProducts', 'GET')),
        catchError(this.showError('getAllProducts'))
      );
  }

  /**
   * GET: get a product from the database by an id
  */
  getProduct(id: string): Observable<Product> {
    const url = `${this.productApi}getProductById?productId=${id}`;
    return this.httpClient
      .get<Product>(url)
      .pipe(tap(_ => this.notify(`get product id=${id}`, 'GET')),
        catchError(this.showError(`getProduct id=${id}`))
      );
  }

  /**
   * POST: add a new product to the database
   */
  addProduct(product: Product): Observable<any> {
    return this.httpClient.post<Product>(`${this.productApi}createProduct`, product, httpOptions);
      // .pipe(tap((product: Product) => this.notify(`new product added id=${product.id}`, 'POST')),
      //   catchError(this.showError('addProduct'))
      // );
  }

  /**
   * PUT: update an existing product to the database
   */
  updateProduct(product: Product): Observable<any> {
    return this.httpClient
      .put(`${this.productApi}updateProduct/${product.id}`, product, httpOptions)
      .pipe(tap(_ => this.notify(`updated product id=${product.id}`, 'PUT')),
        catchError(this.showError('updateProduct'))
      );
  }

  /**
   * DELETE: delete an existing product from the database
   */
  deleteProduct(product: Product | string): Observable<any> {
    const id = typeof product === 'string' ? product : product.id;
    const url = `${this.productApi}deleteProduct/${id}`;
    return this.httpClient
      .delete<Product>(url, httpOptions)
      .pipe(tap(_ => this.notify(`deleted product id=${id}`, 'DELETE')),
        catchError(this.showError('deleteProduct'))
      );
  }

  /**
   * Toast service to notify message
   */
  protected notify(message: string, method: string) {
    this.toastService.openSnackBar(message, method);
  }

  /**
  * Erorr: show exception message
  */
  protected showError(method: string) {
    return function errorHandler(res: HttpErrorResponse) {
      const message = res.message || '';
      const error = `${method} Error${message ? ': ' + message : ''}`;
      this.notify(error, method);
    }.bind(this);
  }
}
