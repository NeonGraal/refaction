import requests
from assertpy import assert_that

BASE_URL = 'http://localhost:58123'

class BaseEndpoints(object):
    BASE_PATH = []

    def ok_url(self, *path, **kwargs):
        r = self.url(*path, **kwargs)

        if not r.ok:
            print(r.text)

        assert_that(r.ok).is_true()
        return {} if r.status_code == 204 else r.json()

    def url(self, *path, method="GET", **kwargs):
        return requests.request(method, '/'.join(self.BASE_PATH + list(path)), **kwargs)

OK_PRODUCT = "de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"
UNKNOWN_PRODUCT = "fcc8bf61-43ef-4581-9ed3-e9ecb04b469c"
NEW_PRODUCT = "ccd8fefc-cd0d-4b5c-b43b-b0322b847b58"

class TestProductsEndpoints(BaseEndpoints):
    BASE_PATH = [BASE_URL, 'products']

    def test_get_all(self):
        data = self.ok_url('')

        assert_that(data).contains_key("Items")

    def test_find(self):
        data = self.ok_url(params=dict(name='Galaxy'))

        assert_that(data).contains_key("Items")

    def test_get_id(self):
        data = self.ok_url(OK_PRODUCT)

        assert_that(data).contains_key("Id", "Name", "Description", "Price", "DeliveryPrice")

    def test_get_unknown(self):
        r = self.url(UNKNOWN_PRODUCT)

        assert_that(r.status_code).is_equal_to(404)

    def test_create(self):
        new_product = dict(Id=NEW_PRODUCT, Name="New Product",
                           Description="Product description", Price=123.45, DeliveryPrice=12.34)
        self.ok_url(method='POST', data=new_product)

    def test_update(self):
        self.ok_url(NEW_PRODUCT, method='PUT', data=dict(Description="New Product description"))

    def test_delete(self):
        self.ok_url(NEW_PRODUCT, method='DELETE')


OK_OPTION = "9ae6f477-a010-4ec9-b6a8-92a85d6c5f03"
UNKNOWN_OPTION = "4016774b-af3d-4d51-95bc-ed7977140b3d"
NEW_OPTION = "1ba78fe7-6dbd-4c8b-8ec1-d3cbba6b56c5"

class TestProductsOptionsEndpoints(BaseEndpoints):
    BASE_PATH = [BASE_URL, 'products', OK_PRODUCT, 'options']

    def test_get_all(self):
        data = self.ok_url('')

        assert_that(data).contains_key('Items')

    def test_get_id(self):
        data = self.ok_url(OK_OPTION)

        assert_that(data).contains_key('Id', 'Name', 'Description')

    def test_get_unknown(self):
        r = self.url(UNKNOWN_OPTION)

        assert_that(r.status_code).is_equal_to(404)

    def test_create(self):
        new_option = dict(Id=NEW_OPTION, ProductID=OK_PRODUCT,
                           Name="New Option", Description="Option description")
        self.ok_url(method='POST', data=new_option)

    def test_update(self):
        self.ok_url(NEW_OPTION, method='PUT', data=dict(Description="New Option description"))

    def test_delete(self):
        self.ok_url(NEW_OPTION, method='DELETE')
