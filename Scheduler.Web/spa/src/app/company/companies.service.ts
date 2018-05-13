import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { IListModel } from '../common/IListModel';

@Injectable()
export class CompaniesService {

    constructor(private http: HttpClient) {
    }

    getList(filter: any): Observable<IListModel> {
        return this.http.get<IListModel>('/api/companies/list', { params: new HttpParams({ fromObject: filter })} );
    }
}


