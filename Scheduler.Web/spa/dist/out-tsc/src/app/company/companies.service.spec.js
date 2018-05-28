"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var testing_1 = require("@angular/core/testing");
var companies_service_1 = require("./companies.service");
describe('CompaniesService', function () {
    beforeEach(function () {
        testing_1.TestBed.configureTestingModule({
            providers: [companies_service_1.CompaniesService]
        });
    });
    it('should be created', testing_1.inject([companies_service_1.CompaniesService], function (service) {
        expect(service).toBeTruthy();
    }));
});
//# sourceMappingURL=companies.service.spec.js.map