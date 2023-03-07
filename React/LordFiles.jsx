import React, { useEffect, useState } from 'react';
import { Row, Card, Container, Col } from 'react-bootstrap';
import Files from './Files';
import locale from 'rc-pagination';
import Pagination from 'rc-pagination';
import filerService from '../../../services/fileService';
import debug from 'sabio-debug';
const _logger = debug.extend('LANDLORD-Files');

function LordFiles() {
    const [pageData, setPageData] = useState({
        files: [],
        filesMapped: [],
        pageIndex: 0,
        pageSize: 6,
        totalCount: 0,
    });

    useEffect(() => {
        filerService.getCurrent(pageData.pageIndex, pageData.pageSize).then(onGetFilesSucces).catch(onGetFilesError);
    }, [pageData.pageIndex]);

    const onGetFilesSucces = (response) => {
        const filesArr = response.item.pagedItems;
        setPageData((prevState) => {
            const newData = { ...prevState };
            newData.files = filesArr;
            newData.filesMapped = filesArr.map(mapFiles);
            return newData;
        });
        setPageData((prevState) => {
            const pageData = { ...prevState };
            pageData.totalCount = response.item.totalCount;
            return pageData;
        });
    };

    const onGetFilesError = (error) => {
        _logger(error);
    };

    const mapFiles = (file, index) => {
        return <Files key={index} file={file} />;
    };

    const onPageChange = (page) => {
        setPageData((prevState) => {
            const currentPageInfo = { ...prevState };
            currentPageInfo.pageIndex = page - 1;
            return currentPageInfo;
        });
    };
    return (
        <>
            <div className="mt-4">
                <Card className="col-12 file-height">
                    <Container>
                        <Row>
                            <Col className="col-12 ms-2 mt-2">
                                <h4 className="header-title mt-1">Files</h4>
                                <Row>{pageData.filesMapped}</Row>
                            </Col>
                        </Row>
                        <Pagination
                            className="position-absolute bottom-0 start-0 m-3"
                            onChange={onPageChange}
                            current={pageData.pageIndex + 1}
                            total={pageData.totalCount}
                            pageSize={pageData.pageSize}
                            locale={locale}
                        />
                    </Container>
                </Card>
            </div>
        </>
    );
}

export default LordFiles;
