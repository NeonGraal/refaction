import requests
from assertpy import assert_that

BASE_URL = 'http://localhost:58123'

OK_PRODUCT = "de1287c0-4b15-4a7b-9d8a-dd21b3cafec3"
NEW_PRODUCT = "ccd8fefc-cd0d-4b5c-b43b-b0322b847b58"


class BaseEndpoints(object):
    BASE_PATH = []

    def check_url(self, *path, method='GET', **kwargs):
        r = requests.request(method, '/'.join(self.BASE_PATH + list(path)), **kwargs)

        assert_that(r.ok).is_true()
        return {} if r.status_code == 204 else r.json()


class TestProductsEndpoints(BaseEndpoints):
    BASE_PATH = [BASE_URL, 'products']

    def test_get_all(self):
        data = self.check_url('')

        assert_that(data).contains_key("Items")

    def test_find(self):
        data = self.check_url(params=dict(name='Galaxy'))

        assert_that(data).contains_key("Items")

    def test_get_id(self):
        data = self.check_url(OK_PRODUCT)

        assert_that(data).contains_key("Id", "Name", "Description", "Price", "DeliveryPrice")

    def test_create(self):
        new_product = dict(Id=NEW_PRODUCT, Name="New Product",
                           Description="Product description", Price=123.45, DeliveryPrice=12.34)
        self.check_url(method='POST', data=new_product)

    def test_update(self):
        self.check_url(NEW_PRODUCT, method='PUT', data=dict(Description="New Product description"))

    def test_delete(self):
        self.check_url(NEW_PRODUCT, method='DELETE')


OK_OPTION = "9ae6f477-a010-4ec9-b6a8-92a85d6c5f03"
NEW_OPTION = "1ba78fe7-6dbd-4c8b-8ec1-d3cbba6b56c5"

class TestProductsOptionsEndpoints(BaseEndpoints):
    BASE_PATH = [BASE_URL, 'products', OK_PRODUCT, 'options']

    def test_get_all(self):
        data = self.check_url('')

        assert_that(data).contains_key('Items')

    def test_get_id(self):
        data = self.check_url(OK_OPTION)

        assert_that(data).contains_key('Id', 'Name', 'Description')

    def test_create(self):
        new_option = dict(Id=NEW_OPTION, ProductID=OK_PRODUCT,
                           Name="New Option", Description="Option description")
        self.check_url(method='POST', data=new_option)

    def test_update(self):
        self.check_url(NEW_OPTION, method='PUT', data=dict(Description="New Option description"))

    def test_delete(self):
        self.check_url(NEW_OPTION, method='DELETE')
