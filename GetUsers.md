# Method :: [GET]/Users/GetUsers

## ����������

��������� ������ ���������� � �������������

## ������� ������

```json
{
    "offset": int, //������� ������� ���������� -- �� ��������� 0 
    "limit": int, //������� ������� ������� -- Required
    "filter": string, //������� ���������� ������� -- �� ��������� "id > 0"
    "sort": string //������ ���������� ������� -- �� ��������� "id desc"
}
```

## �������� ������
```json
[
    {
        "id": 3,
        "createDate": "2024-09-04T07:37:34.591718Z",
        "editDate": "2024-09-04T07:37:34.591718Z",
        "login": "2",
        "gender": "�������",
        "city": "�������",
        "firstname": "�����",
        "lastname": "���������",
        "cooperationType": "�� �������",
        "type": "������",
        "contact": "�����",
        "patronymic": null,
        "fullname": "����� ���������",
        "discription": "������ ���� ������ �����. ������ ����� ��� �����",
        "avatar": "",
        "isNude": false
    },
    {
        "id": 2,
        "createDate": "2024-09-04T07:37:07.488074Z",
        "editDate": "2024-09-04T07:37:07.488074Z",
        "login": "1",
        "gender": "�������",
        "city": "�����������",
        "firstname": "������",
        "lastname": "���������",
        "cooperationType": "�� �������",
        "type": "��������",
        "contact": "@kravectv",
        "patronymic": null,
        "fullname": "������ ���������",
        "discription": "������ ���� ������ �����. ������ ����� ��� �����",
        "avatar": "",
        "isNude": false
    }
]
```

## ������ ������

```bash
curl -L -X GET 'http://38.135.55.111:5000/Users/?sort=id asc&filter=gender eq 2&limit=5'
```

## ������ ������

200

```bash
[
    {
        "id": 3,
        "createDate": "2024-09-04T07:37:34.591718Z",
        "editDate": "2024-09-04T07:37:34.591718Z",
        "login": "2",
        "gender": "�������",
        "city": "�������",
        "firstname": "�����",
        "lastname": "���������",
        "cooperationType": "�� �������",
        "type": "������",
        "contact": "�����",
        "patronymic": null,
        "fullname": "����� ���������",
        "discription": "������ ���� ������ �����. ������ ����� ��� �����",
        "avatar": "",
        "isNude": false
    }
]
```

## ��������
1. ��������� ���������� ������������ �����. ���� ���� �� ��������� - ������� BadRequest
2. ��������� ������� � ���������� ������ session � �������. � ������ ������� ������� Unauthorized.
3. ������� ������ �������������. ���� ������������ �� ������� ������� ������ ������.