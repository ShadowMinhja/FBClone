import React from 'react'
import LoadScript from '../../utils/LoadScript'

import styles from './styles.css'

//This is a regular React component to handle jQuery injection (Footer Guarantees the DOM was loaded)
export class Footer extends React.Component {

    constructor() {
        super();
        this.LoadScript = LoadScript;
    }

    componentDidMount() {
        this.LoadScript.prototype.load('/Scripts/app/global.js', function(){}); //Initialize jQuery Controls (mix it up)
    }
    
    render() {
        return (    
            <footer id="footer">
	            <div className="footer-main">
		            <div className="container">
			            <div className="row">

				            <div className="col-md-offset-2 col-md-3">
                                <div className="widget widget-about">
                                    <h2>fbClone</h2>
                                    <p>We're always happy to hear from you about our product.  We're working hard to improve it day by day, and hope you enjoy it.</p>
                                </div>
					            
				            </div>

				            <div className="col-md-3">
					            <div className="widget widget-about">
						            <h2>about us</h2>
                                    {/*<p>We are food lovers!</p>*/}
                                </div>

				            </div>

                            <div className="col-md-3">
                                <div className="widget mt-20-md">
						            <h2>product</h2>
						            {/*<p>Bringing you the best food choices, by the friends and family you trust.</p>*/}
					            </div>
                                {/*
					            <div className="widget mt-20-md">
						            <h2><a href="/restauranteurs">restauranteurs</a></h2>
						            <p>Do you own an establishment? Check inside for how fbClone can help you connect to your customers!</p>
					            </div>
                                */}
				            </div>

                            <div className="col-md-2">
                                <div className="widget widget-contact mt-20-md">
                                    <h2>contact us</h2>
                                    <address>
                                        <strong>Email:</strong> <a href="mailto:support@fbClone.io">support@fbClone.io</a><br />
                                    </address>
                                </div>
                            </div>

			            </div>
		            </div>
	            </div>

	            <div className="footer-bottom">
		            <div className="container">
			            <div className="row">

				            <div className="col-md-4 copyright">
					            <p className="mb-0">&copy; Copyright 2016 by <a href="#">fbClone</a>. All Rights Reserved.</p>
					            <p><a href="#">Terms of use</a> / <a href="#">Privacy Policy</a></p>
				            </div>

				            <div className="col-md-8 text-right text-center-md">

					            <a className="social-icon social-facebook" href="#">
						             <div className="front">
							            <i className="fa fa-2x fa-facebook"></i>
						             </div>
						             <div className="back">
							            <i className="fa fa-2x fa-facebook"></i>
						             </div>
					            </a>

					            <a className="social-icon social-googleplus" href="#">
						             <div className="front">
							            <i className="fa fa-2x fa-google-plus"></i>
						             </div>
						             <div className="back">
							            <i className="fa fa-2x fa-google-plus"></i>
						             </div>
					            </a>
                                
					            <a className="social-icon social-twitter" href="#">
						             <div className="front">
							            <i className="fa fa-2x fa-twitter"></i>
						             </div>
						             <div className="back">
							            <i className="fa fa-2x fa-twitter"></i>
						             </div>
					            </a>

				            </div>

			            </div>
		            </div>
	            </div>

            </footer>
        );
    }
}

export default Footer;