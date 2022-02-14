import InvoiceFormStateBase from './base/InvoiceFormStateBase.js';

class DefaultInvoiceFormState extends InvoiceFormStateBase {
    #defaultActionAttribute = '/Invoice/InvoicesByDateRange';

    constructor() {
        super();
        this.State = this.#getFormState();
        this.Action = this.#defaultActionAttribute;
    }

    #getFormState() {
        let formState =
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
            `;
        return formState;
    }
}

export default DefaultInvoiceFormState;