import SearchByDateRangeInvoiceFormStateDecorator from './../SearchByDateRangeInvoiceFormStateDecorator.js';
import SearchByNameAndDateInvoiceFormStateDecorator from './../SearchByNameAndDateInvoiceFormStateDecorator.js';

class InvoiceFormStateDecoratorFactory {

    get decoratorTypes() {
        return {
            SearchByDateRangeInvoiceFormStateDecorator,
            SearchByNameAndDateInvoiceFormStateDecorator
        }
    }


    static createInvoiceFormStateDecorator(decorator, invoiceFormStateBase) {
        let decoratorBase = this.prototype.decoratorTypes[decorator];

        return new decoratorBase(invoiceFormStateBase);
    }
}

export default InvoiceFormStateDecoratorFactory;