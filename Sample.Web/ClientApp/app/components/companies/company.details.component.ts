import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, Inject, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Location } from '@angular/common';


@Component({
    selector: 'company-details',
    templateUrl: './company.details.component.html',
    styleUrls: ['./forms.css']
})
export class CompanyDetailsComponent implements OnInit {

    @Input()
    public model: CompanyDetailsModel;

    constructor(
        private http: HttpClient,
        @Inject('BASE_URL') private baseUrl: string,
        private route: ActivatedRoute,
        private location: Location
    ) {


    }


    ngOnInit() {

        const id = this.route.snapshot.paramMap.get('id');

        this.http
            .get<CompanyDetailsModel>(this.baseUrl + 'api/companies/details/' + id)
            .subscribe(result => {
                this.model = result.json() as CompanyDetailsModel;

                if (this.model.data == null)
                {
                    alert("Entry not found");
                    this.goBack();
                }
            },
            error => console.error(error)
            );
    }

    goBack() {
        this.location.back();
    }

    onSubmit() {
        this.http
            .post(this.baseUrl + 'api/companies/save', this.model.data)
            .subscribe(r => {
                var d = r.json()
                if (d.success) {
                    this.goBack();
                }
            }, error => console.error(error));
    }
}


class CompanyDetailsInfo {
    id: number;
    title: string;
    isActive: boolean;
}

class CompanyDetailsModel {
    data: CompanyDetailsInfo
}