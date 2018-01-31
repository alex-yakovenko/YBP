import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'companieslist',
    templateUrl: './companies.component.html'
})
export class CompaniesComponent {

    public list: CompanyInfo[];

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {

        http
            .get(baseUrl + 'api/companies/list')
            .subscribe(
            result => {
                var d = result.json() as CompanyListModel;
                console.log(d);
                    this.list = d.items;
                },
                error => console.error(error)
            );

    }


    public delete(id: number) {
        alert(id);
    }
}


class CompanyInfo {
    id: number;
    title: string;
    isActive: boolean;
}

class CompanyListModel {
    items: CompanyInfo[];
    totalCount: number;
}