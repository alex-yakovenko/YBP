import { Component, Input, Injectable } from '@angular/core';

@Component({
  selector: 'pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss']
})
export class PaginationComponent {

    @Input() control: PaginationControlInfo;
    
}


@Injectable()
export class Pagination {

    static readonly DefaultRows: number = 20;

    private page: number; // 1 based page index
    private total: number;
    public rows: number;
    public order: string;
    public desc: boolean;

    private filter: any;

    public control: PaginationControlInfo;

    constructor() {
        this.control = new PaginationControlInfo(0, 0, 0);
        this.rows = Pagination.DefaultRows;
    }

    public listParams(): ListParams {
        return new ListParams(
            this.getSkipCount(),
            this.rows,
            this.order || '',
            this.desc || false
        )
    }

    public parseRouteParam(k: string, val: string): boolean {
        switch (k) {
            case "p.rows":
                this.rows = +val || 20;
                return true;

            case "p.order":
                this.order = val;
                return true;

            case "p.desc":
                this.desc = val == "true";
                return true;

            default:
                return false;
        }
    }

    public getUrlParams(): any {
        let f = {};

        let names = ['page', 'rows', 'order', 'desc'];
        for (let n of names)
            if (this[n])
                f['p.' + n] = this[n];0

        return this.clearDefaultValues(f);
    }

    private getSkipCount(): number {
        return (this.page - 1) * this.rows;
    }

    public setTotal(total: number) {
        this.total = total;
        this.control = this.getControlInfo();
    }


    public setPage(pageNum: number) {
        this.page = pageNum;
        this.control = this.getControlInfo();
    }

    private generateQueryParams(prmDif: any): any {

        return this.clearDefaultValues(Object.assign({}, this.filter, this.getUrlParams(), prmDif)); 

    }

    private clearDefaultValues(f): any {
        if (f['p.page'] && +f['p.page'] < 2)
            f['p.page'] = null;

        if (f['p.rows'] && f['p.rows'] == Pagination.DefaultRows)
            f['p.rows'] = null;

        if (f['p.desc'] == false)
            f['p.desc'] = null;

        return f;
    }

    public getControlInfo(): PaginationControlInfo {

        const nums = 7;

        let skipCount = this.getSkipCount();

        let takeCount = skipCount + this.rows;
        if (takeCount > this.total)
            takeCount = this.total;

        let result = new PaginationControlInfo(
            skipCount + 1,
            takeCount,
            this.total
        );


        if (this.page < 1)
            this.page = 1;

        let d = this.total / this.rows;

        let totalPages = Math.ceil(d);

        let halfOfNum = Math.ceil(nums / 2);

        if (totalPages > 1) {

            if (this.page > totalPages)
                this.page = totalPages;

            let firstPageNum = this.page - halfOfNum;

            if (firstPageNum < 0)
                firstPageNum = 0;

            let lastPageNum = firstPageNum + nums - 1;
            if (lastPageNum > totalPages - 1)
                lastPageNum = totalPages - 1;

            if (firstPageNum > 0)
                result.links.push(new PaginationLinkInfo(
                    false,
                    "First",
                    0,
                    true,
                    false,
                    this.generateQueryParams({ 'p.page': null })
                ));

            if (this.page > 1)
                result.links.push(new PaginationLinkInfo(
                    false,
                    "Previous",
                    this.page - 1,
                    true,
                    false,
                    this.generateQueryParams({ 'p.page': this.page - 1 })
                ));

            for (let i = firstPageNum; i <= lastPageNum; i++)
                result.links.push(new PaginationLinkInfo(
                    this.page == i + 1,
                    (i + 1).toString(),
                    i,
                    false,
                    false,
                    this.generateQueryParams({ 'p.page': i + 1 })
                ));

            if (this.page < totalPages)
                result.links.push(new PaginationLinkInfo(
                    false,
                    "Next",
                    this.page + 1,
                    false,
                    true,
                    this.generateQueryParams({ 'p.page': this.page + 1 })
                ));

            if ((this.page + halfOfNum) <= totalPages)
                result.links.push(new PaginationLinkInfo(
                    false,
                    "Last",
                    totalPages,
                    false,
                    true,
                    this.generateQueryParams({ 'p.page': totalPages })
                ));

        }

        return result;
    }

    applyUrlData(prms: any) {
        this.filter = prms;
        this.control = this.getControlInfo();
    }

}

export class ListParams {
    constructor(
        public skipCount: number,
        public takeCount: number,
        public sortOrder: string,
        public sortDesc: boolean) {
    }
}


export class PaginationControlInfo {
    public links: PaginationLinkInfo[]
    constructor(
        public startIndex: number,
        public endIndex: number,
        public total: number
    ) {
        this.links = [];
    }
}

export class PaginationLinkInfo {

    constructor(
        public isActive: boolean,
        public caption: string,
        public page: number,
        public isPrevious: boolean,
        public isNext: boolean,
        public queryParams: string
    ) { }
}