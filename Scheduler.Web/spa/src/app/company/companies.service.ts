import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { IListModel } from '../common/IListModel';

@Injectable()
export class CompaniesService {

    constructor(private http: HttpClient) {
    }

    getList(): Observable<IListModel<ICompanyListItem>> {
        return this.http.get<IListModel<ICompanyListItem>>('/api/companies/list');
    }
}

export interface ICompanyListItem {
    id: number;
    title: string;
    isApproved: boolean;
}