import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IProduct } from '../shared/models/product';
import { IBrand } from '../shared/models/brand';
import { IProductType } from '../shared/models/productType';
import { IPagination } from '../shared/models/pagination';
import { map } from 'rxjs/operators';
import { ShopParams } from '../shared/models/shopParams';

@Injectable({
  providedIn: 'root',
})
export class ShopService {
  products: IProduct[];
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) {}

  getProduct(id: number) {
    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }

  getProducts(shopParams: ShopParams) {
    //let used to reasign variable.
    let params = new HttpParams();

    if (shopParams.brandId !== 0) {
      params = params.append('brandId', shopParams.brandId.toString());
    }

    if (shopParams.typeId !== 0) {
      params = params.append('typeId', shopParams.typeId.toString());
    }

    if (shopParams.search) {
      console.log(shopParams.search);
      params = params.append('search', shopParams.search);
    }

    params = params.append('sort', shopParams.sort);
    params = params.append('pageIndex', shopParams.pageNumber.toString());
    params = params.append('pageSize', shopParams.pageSize.toString());

    return (
      this.http
        .get<IPagination>(this.baseUrl + 'products', {
          observe: 'response',
          params,
        })
        // this {} is the options paramater for the response call
        .pipe(
          map((response) => {
            return response.body;
            // we can add many things to this pipe, such as delay
            // brings in response body
            // map is key as it lets us map the observed response seen above to IPagination
            // so we map in our response and get response.body
          })
        )
    );
    // so what observe does it to grab the actuall http response. As such we have to extract
    // the body out from this.
    // we are getting an observable back from the http request
    // get the observable and project it into an IPagination object, alongside its params.
  }

  getBrands() {
    return this.http.get<IBrand[]>(this.baseUrl + 'products/brands');
  }

  getProductTypes() {
    return this.http.get<IProductType[]>(this.baseUrl + 'products/types');
  }
}
