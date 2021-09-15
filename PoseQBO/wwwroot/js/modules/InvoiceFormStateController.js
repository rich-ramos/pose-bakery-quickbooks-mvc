import InvoiceFormState from "../invoice-form-state/component/InvoiceFormState.js";
import FormProcessor from "./FormProcessor.js";
import SearchByDateRangeInvoiceFormStateDecorator from "../invoice-form-state/decorators/SearchByDateRangeInvoiceFormStateDecorator.js";
import SearchByNameAndDateInvoiceFormStateDecorator from "../invoice-form-state/decorators/SearchByNameAndDateInvoiceFormStateDecorator.js";

class InvoiceFormStateController {
    #linkId = 'name-link';
    #nameAndDateActionAttributeValue = '/QBO/InvoicesByNameAndDate';
    #dateRangeActionAttributeValue = '/QBO/InvoicesByDateRange';

    constructor() {
        this.form = document.querySelector('#invoiceForm');
        this.invoiceFormState = new InvoiceFormState();
        this.formProcessor = null;
        this.invoiceNavLinks = document.querySelectorAll('.invoice-nav-link');
        this.events();
    }

    events() {
        this.invoiceNavLinks.forEach((invoiceNavLink) => {
            invoiceNavLink.addEventListener('click', (e) => this.handleClick(e));
        });
    }

    handleClick(e) {
        this.invoiceNavLinks.forEach((invoiceNavLink) => {
            invoiceNavLink.classList.remove('active');
        });
        e.target.classList.add('active');
        this.changeFormState(e.target.id);
    }

    changeFormState(id) {
        if (id == this.#linkId) {
            const nameAndDateInvoiceFormStateDecorator = new SearchByNameAndDateInvoiceFormStateDecorator(this.invoiceFormState);
            this.formProcessor = new FormProcessor(this.form, nameAndDateInvoiceFormStateDecorator);
            this.formProcessor.setFormState();
            this.formProcessor.setActionAttributeValue(this.#nameAndDateActionAttributeValue);
        } else {
            const dateRangeInvoiceFormStateDecorator = new SearchByDateRangeInvoiceFormStateDecorator(this.invoiceFormState);
            this.formProcessor = new FormProcessor(this.form, dateRangeInvoiceFormStateDecorator);
            this.formProcessor.setFormState();
            this.formProcessor.setActionAttributeValue(this.#dateRangeActionAttributeValue);
        }
    }
}

export default InvoiceFormStateController;