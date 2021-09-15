import InvoiceFormStateDecoratorBase from './base/InvoiceFormStateDecoratorBase.js';

class SearchByDateRangeInvoiceFormStateDecorator extends InvoiceFormStateDecoratorBase {
    constructor(invoiceFormState) {
        super(invoiceFormState);
        const newFormState =
            `
                <div class="form-row">
                    <div class="form-group col-md-6">
                        <label for="startDate">Start Date</label>
                        <input type="date" id="startDate" name="startDate" class="form-control" required />
                        <div class="invalid-feedback">
                            Start date must not be greater than end date or equal
                        </div>
                    </div>
                    <div class="form-group col-md-6">
                        <label for="endDate">End Date</label>
                        <input type="date" id="endDate" name="endDate" class="form-control" required />
                        <div class="invalid-feedback">
                            End date must not be less than start date or equal
                        </div>
                    </div>
                </div>
                <div class="text-center m-2">
                    <button class="btn btn-primary text-center" type="submit">Submit</button>
                </div>
            `;
        this.State = newFormState;
    }

    get State() {
        return super.State;
    }

    set State(state) {
        super.State = state;
    }
}

export default SearchByDateRangeInvoiceFormStateDecorator;