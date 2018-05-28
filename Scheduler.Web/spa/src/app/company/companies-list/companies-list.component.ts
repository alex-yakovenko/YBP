import { Component, OnInit, Inject } from '@angular/core';
import { CompaniesService } from '../companies.service';
import { IListModel } from '../../common/IListModel';
import { ActivatedRoute, Router } from '@angular/router';
import { Pagination, PaginationControlInfo } from '../../layout/pagination/pagination.component';

@Component({
    selector: 'app-companies-list',
    templateUrl: './companies-list.component.html',
    styleUrls: ['./companies-list.component.scss']
})
export class CompaniesListComponent implements OnInit {

    public data: IListModel;

    public filter: any;

    constructor(
        private service: CompaniesService,
        private route: ActivatedRoute,
        private router: Router,
        public page: Pagination
    ) {
        this.filter = { title: null };
        this.data = <IListModel>{};
    }

    ngOnInit() {

        this.data = { "totalCount": 5, "items": [{ "id": 1, "title": "Company 1", "isApproved": true }, { "id": 2, "title": "Company 2", "isApproved": true }, { "id": 3, "title": "Company 3", "isApproved": true }, { "id": 4, "title": "Company e5beb3b5-d45c-4e1e-9787-27ff213b8508", "isApproved": true }, { "id": 5, "title": "jgfjgj2", "isApproved": false }] };

        this.route.queryParamMap.subscribe((prm) => {

            this.page.setPage(5);
        
            this.page.setTotal(1215);

        });

        this.page.applyUrlData({});

        return;

        this.route.queryParamMap.subscribe((prm) => {
            for (let k of prm.keys) {

                var val = prm.get(k);

                if (!this.page.parseRouteParam(k, val))
                    this.filter[k] = val;

            }

            var p = Object.assign({}, this.filter, this.page.listParams());

            this.service.getList(p)
                .subscribe((result: any) => {
                    Object.assign(this.data, result);
                    this.page.setTotal(result.total);
                });

            this.page.applyUrlData(this.filter);

        });
        
    }


    public applySearch() {

        var f = {};

        var names = Object.getOwnPropertyNames(this.filter);

        for (let n of names)
            if (this.filter[n])
                f[n] = this.filter[n];

        Object.assign(f, this.page.getUrlParams())

        this.router
            .navigate([this.route.routeConfig.path], {
                queryParams: f
            });
    }

    private searchTimeout: any;
    private prevFltrJson: string;

    public filterChanged() {

        var fltrJson = JSON.stringify(this.filter);

        if (fltrJson == this.prevFltrJson)
            return;

        this.prevFltrJson = fltrJson;

        if (this.searchTimeout) {
            clearTimeout(this.searchTimeout);
            this.searchTimeout = null;
        }

        this.searchTimeout = setTimeout(() => {
            this.searchTimeout = null;
            this.applySearch();
            this.page.applyUrlData(this.filter);
        }, 800);
    }

    public clearTitle() {
        this.filter.title = '';
        this.filterChanged();
    }

}
