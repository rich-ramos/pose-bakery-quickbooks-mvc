import InvoiceFormStateDecoratorBase from './base/InvoiceFormStateDecoratorBase.js';

class SearchByDateRangeInvoiceFormStateDecorator extends InvoiceFormStateDecoratorBase {
    constructor(invoiceFormState) {
        super(invoiceFormState);
        const newFormState =
            `
                <div class="form-row">
                    <div class="form-group col-md-6">
                        <label for="startDate">Start Date</label>
                        <input type="date" name="startDate" class="form-control" />
                    </div>
                    <div class="form-group col-md-6">
                        <label for="endDate">End Date</label>
                        <input type="date" name="endDate" class="form-control" />
                    </div>
                </div>
                <div class="text-center m-2">
                    <button class="btn btn-primary text-center" type="submit">Submit</button>
                </div>
            `;
        this.invoiceFormState.State = newFormState;
    }

    get State() {
        return super.State;
    }
}

export default SearchByDateRangeInvoiceFormStateDecorator;