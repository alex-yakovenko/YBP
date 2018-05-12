import { Inject } from '@angular/core';
import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'companieslist',
    templateUrl: './companies.component.html'
})
export class CompaniesComponent {

    public list: CompanyInfo[];

    constructor(
        public http: HttpClient,

        @Inject('BASE_URL')
        public baseUrl: string) {

        http
            .get(this.baseUrl + 'api/companies/list')
            .subscribe(
                result => {
                    console.log(result);
                    this.list = result.items;
                    }
            );

    }


    public delete(id: number) {
        if (confirm("Remove company?"))
        {

        };
    }
}


export class CompanyListModel {
    items: CompanyInfo[];
    totalCount: number;
}

export class CompanyInfo {
    id: number;
    title: string;
    isActive: boolean;
}



