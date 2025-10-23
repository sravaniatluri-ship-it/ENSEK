# ?? ENSEK API Defect Report

## 1. Buy Endpoint

### DEFECT-001: Missing Energy Type Name
**Description:**  
The `/ENSEK/buy` response does not include the energy type name, making it unclear which energy type each item represents.

---

### DEFECT-002: Missing Unique Identifier for Energy Types
**Description:**  
The `/ENSEK/buy` response does not return a unique identifier (`id`) for each energy type.  
This creates ambiguity when calling subsequent endpoints such as `/ENSEK/buy/{id}/{quantity}`, which require an integer `id`.

---

## 2. Orders Endpoint

### DEFECT-003: Duplicate Order IDs
**Description:**  
The same order ID (`31fc32ab-bccb-44ab-9352-4f43dc4ed4eb`) appears multiple times in `/ENSEK/orders`.  
Each order should have a unique identifier, but this duplication violates data integrity.

---

### DEFECT-004: Single Order ID Used for Multiple Energy Types
**Description:**  
Order ID `0aa0262b-df48-40f8-90f1-281f2bf5f26d` appears under both gas and electric categories.  
Each order should be tied to only one energy type for consistency and traceability.

---

## 3. Orders/Ordered Endpoint

### DEFECT-005: Missing Validation and Error Handling
**Description:**  
The `/ENSEK/orders/ordered` endpoint accepts invalid quantities (e.g., negative, decimal, or unrealistic numbers) without validation or error feedback.  
The API should enforce quantity validation and return appropriate error messages.

---

## 4. Get /ENSEK/order/ordered

### DEFECT-006: Data Inconsistency Between Endpoints
**Description:**  
The order details (e.g., fuel type and quantity) returned by `GET /ENSEK/order/ordered` do not appear in the list of orders retrieved via `GET /ENSEK/orders`.  
This suggests inconsistency between the single-order and all-orders endpoints.

---

### DEFECT-007: Missing 404 or Validation for Invalid Order IDs
**Description:**  
When invalid or non-existent order IDs are entered, the API still returns HTTP 200 instead of an appropriate 404 or validation error.  
Proper status codes should be implemented for nonexistent resources.

---

### DEFECT-008: Mismatched Data Between List and Detail Views
**Description:**  
The data from `GET /ENSEK/orders` (list view) does not match the corresponding data from `GET /ENSEK/orders/{id}` (detail view).  
Order names and quantities differ, compromising data reliability.

### DEFECT-009: Date Parsing Issue Causing Failure on Specific Record
**Description:** 
Root cause appears to be a date parsing issue — the 5th record uses a single-digit day (“7 Feb”) which fails the `TryParseExact("dd MMM")` format expectation.


## 5. DELETE /ENSEK/orders/{orderId}

### DEFECT-009: Unable to Delete Order – 500 Internal Server Error
**ID:** API-DEL-001  
**Severity:** ?? High  
**Priority:** P1  
**Status:** Open  

**Summary:**  
When deleting an existing order via `DELETE /ENSEK/orders/{orderId}`, the API returns a `500 Internal Server Error`, preventing successful deletion.

**Environment:**  
- **API Base URL:** https://qacandidatestest.smok.io/ENSEK/orders  
- **Authentication:** Bearer token (valid)  
- **Content-Type:** application/json  

**Steps to Reproduce:**
1. Execute `GET /ENSEK/orders`.
2. Copy a valid `orderId` (e.g., `31fc32ab-bccb-44ab-9352-4f43dc4ed4eb`).
3. Execute `DELETE /ENSEK/orders/{orderId}` in Swagger.
4. Observe the response.

**Expected Result:**  
The selected order should be deleted successfully, returning a confirmation message 

**Actual Result:**  
The API returns a `500 Internal Server Error`.  

